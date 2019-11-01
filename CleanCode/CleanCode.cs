using LAIR.ResourceAPIs.WordNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.CodeDom.Compiler;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using NetSpell.SpellChecker;

namespace CleanCode
{
    public partial class CleanCode : Form
    {
        private WordNetEngine WordNetEngine;
        List<Error> ErrorList = new List<Error>();
        OpenFileDialog openFile = new OpenFileDialog();

        public CleanCode()
        {
            InitializeComponent();
            WordNetEngine = new WordNetEngine(Environment.CurrentDirectory + @"\resources\", false);
            FilePath.Text = Environment.CurrentDirectory + @"\text.txt";
        }

        public class Error
        {
            public int StartLine { get; set; }
            public int EndLine { get; set; }
            public string Type { get; set; }
            public string ErrorMessage { get; set; }
            public string Name { get; set; }
        }

        private void GetFile_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                FilePath.Text = openFile.FileName;
            }
            if (!File.Exists(openFile.FileName))
            {
                MessageBox.Show("فایل مورد نظر یافت نشد");
            }
        }

        private void ButtonRun_Click(object sender, EventArgs e)
        {
            ErrorList.Clear();
            string file = File.ReadAllText(openFile.FileName);
            Analyze(file);
            RefreshGrid();
        }
        private void Analyze(string text)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(text);

            var FieldStatement = syntaxTree.GetRoot().DescendantNodes().OfType<VariableDeclarationSyntax>();
            var MethodStatemant = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();
            var IfStatement = syntaxTree.GetRoot().DescendantNodes().OfType<IfStatementSyntax>();
            var WhileStatement = syntaxTree.GetRoot().DescendantNodes().OfType<WhileStatementSyntax>();

           
            



            progressBar.Value = 10;
            foreach (var node in FieldStatement)
            {
              AnalyzeField(node);
            }
            progressBar.Value = 30;
            foreach (var node in MethodStatemant)
            {
                AnalyzeMethod(node);
            }
            progressBar.Value = 60;
            foreach (var node in IfStatement)
            {
                AnalyzeIf(node);
            }
            progressBar.Value = 80;
            foreach (var node in WhileStatement)
            {
                AnalyzeWhile(node);
            }
            progressBar.Value = 100;
        }



 

        private void CleanCode_Load(object sender, EventArgs e)
        {
            openFile.FileName = FilePath.Text;
        }
       
        private void AnalyzeMethod(MethodDeclarationSyntax node)
{
        var startline = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
        var endline = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;
        var singularity = node.DescendantNodes().OfType<InvocationExpressionSyntax>().Count();
          
    try
    {

        AnalyzeName(node.Identifier.Text, WordNetEngine.POS.Verb);

        if (node.ParameterList.Parameters.Count > 4)
        {
            ErrorList.Add(new Error() { StartLine = startline, EndLine = endline, ErrorMessage = "Parameters Are More Than 4", Name = node.Identifier.Text, Type = "MethodDeclarationSyntax" });
        }


        if (endline - startline > 24)
        {
            ErrorList.Add(new Error() { StartLine = startline, EndLine = endline, ErrorMessage = "Screen Size Error", Name = node.Identifier.Text, Type = "MethodDeclarationSyntax" });

        }
                if (singularity > 1)
                {
                    ErrorList.Add(new Error() { StartLine = startline, EndLine = endline, ErrorMessage = "Method Singularity Error", Name = node.Identifier.Text, Type = "MethodDeclarationSyntax" });

                }
            }

    catch (Exception e)
    {
        ErrorList.Add(new Error() { StartLine = startline, EndLine = endline, ErrorMessage = e.Message, Name = node.Identifier.Text, Type = "MethodDeclarationSyntax" });
    }
}
private void AnalyzeField(VariableDeclarationSyntax node)
{
    foreach (var field in node.Variables)
    {
        var startline = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
        var endline = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;
        try
        {
            AnalyzeName(field.Identifier.Text, WordNetEngine.POS.Noun);

        }
        catch (Exception e)
        {

            ErrorList.Add(new Error() { StartLine = startline, EndLine = endline, ErrorMessage = e.Message, Name = field.Identifier.Text, Type = "FieldDeclarationSyntax" });
        }

    }
}
private void AnalyzeIf(IfStatementSyntax node)
{
    var startline = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
    var endline = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;
    var Level = node.DescendantNodes().OfType<IfStatementSyntax>().LongCount<IfStatementSyntax>();
    if (Level > 1)
    {
        ErrorList.Add(new Error() { StartLine = startline, EndLine = endline, ErrorMessage = "More Than Two Indent Level", Name = "IF", Type = "IfStatementSyntax" });
    }
}
private void AnalyzeWhile(WhileStatementSyntax node)
{
    var startline = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
    var endline = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;

}
private void AnalyzeName(string text, WordNetEngine.POS pOS)
{
    if (text.Length < 2)
    {

        throw new Exception("Length is less than 2 char");
    }

    Spelling oSpell = new Spelling();
    var words = Regex.Split(text, @"([A-Z _][a-z]+)");

    foreach (var word in words.Where(c => !string.IsNullOrEmpty(c) && c != "_"))
    {
        if (!oSpell.TestWord(word))
        {
            throw new Exception("FieldName Is not a Valid Word");
        }
        if (pOS == WordNetEngine.POS.Noun)
        {
            SynSet synset = WordNetEngine.GetMostCommonSynSet(word, pOS);
            if (synset == null)
            {
                throw new Exception("FieldName Is Not A Valid noun");
            }
        }
    }
    if (pOS == WordNetEngine.POS.Verb)
    {
        var word = string.Join(" ", words.Where<string>(c => c.Length > 0));
        SynSet synset = WordNetEngine.GetMostCommonSynSet(word, pOS);
        if (synset == null)
        {
            throw new Exception("MethodName Is Not A Valid Verb");
        }
    }

}
private void RefreshGrid()
{
    string json = Newtonsoft.Json.JsonConvert.SerializeObject(ErrorList);
    dynamic dynamic = JsonConvert.DeserializeObject(json);
    dataGridView.DataSource = dynamic;
}
private void comp_Load(object sender, EventArgs e)
{
    openFile.FileName = FilePath.Text;
}

    }
}
