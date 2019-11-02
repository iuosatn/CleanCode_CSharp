using LAIR.ResourceAPIs.WordNet;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NetSpell.SpellChecker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
            public int startLine { get; set; }
            public int endLine { get; set; }
            public string Type { get; set; }
            public string ErrorMessage { get; set; }
            public string Name { get; set; }
        }

        private void Get(object sender, EventArgs e)
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

        private void Run(object sender, EventArgs e)
        {
            ErrorList.Clear();
            string file = File.ReadAllText(openFile.FileName);
            Analyze(file);
            Refresh();
        }
        private void Analyze(string text)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(text);
            var FieldStatement = syntaxTree.GetRoot().DescendantNodes().OfType<VariableDeclarationSyntax>();
            var MethodStatement = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();
            var Statement = syntaxTree.GetRoot().DescendantNodes().OfType<IfStatementSyntax>();
            var WhileStatement = syntaxTree.GetRoot().DescendantNodes().OfType<WhileStatementSyntax>();
            progressBar.Value = 10;
            foreach (var node in FieldStatement)
            {
              AnalyzeField(node);
            }
            progressBar.Value = 30;
            foreach (var node in MethodStatement)
            {
                Analyze(node);
            }
            progressBar.Value = 60;
            foreach (var node in Statement)
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



 

        private void lade(object sender, EventArgs e)
        {
            openFile.FileName = FilePath.Text;
        }
       
        private void Analyze(MethodDeclarationSyntax node)
{
        var startLine = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
        var endLine = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;
        var singularity = node.DescendantNodes().OfType<InvocationExpressionSyntax>().Count();
            foreach (var item in node.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
               string a = item.GetType().ToString();
            }
          
    try
    {

        AnalyzeName(node.Identifier.Text, WordNetEngine.POS.Verb);

        if (node.ParameterList.Parameters.Count > 4)
        {
            ErrorList.Add(new Error() { startLine = startLine, endLine = endLine, ErrorMessage = "Parameters Are More Than 4", Name = node.Identifier.Text, Type = "MethodDeclarationSyntax" });
        }


        if (endLine - startLine > 24)
        {
            ErrorList.Add(new Error() { startLine = startLine, endLine = endLine, ErrorMessage = "Screen Size Error", Name = node.Identifier.Text, Type = "MethodDeclarationSyntax" });

        }
                if (singularity > 1)
                {
                    ErrorList.Add(new Error() { startLine = startLine, endLine = endLine, ErrorMessage = "Method Singularity Error", Name = node.Identifier.Text, Type = "MethodDeclarationSyntax" });

                }
            }

    catch (Exception e)
    {
        ErrorList.Add(new Error() { startLine = startLine, endLine = endLine, ErrorMessage = e.Message, Name = node.Identifier.Text, Type = "MethodDeclarationSyntax" });
    }
}
private void AnalyzeField(VariableDeclarationSyntax node)
{
    foreach (var field in node.Variables)
    {
        var startLine = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
        var endLine = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;
        try
        {
            AnalyzeName(field.Identifier.Text, WordNetEngine.POS.Noun);

        }
        catch (Exception e)
        {

            ErrorList.Add(new Error() { startLine = startLine, endLine = endLine, ErrorMessage = e.Message, Name = field.Identifier.Text, Type = "FieldDeclarationSyntax" });
        }

    }
}
private void AnalyzeIf(IfStatementSyntax node)
{
    var startLine = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
    var endLine = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;
    var Level = node.DescendantNodes().OfType<IfStatementSyntax>().LongCount<IfStatementSyntax>();
    if (Level > 1)
    {
        ErrorList.Add(new Error() { startLine = startLine, endLine = endLine, ErrorMessage = "More Than Two Indent Level", Name = "IF", Type = "IfStatementSyntax" });
    }
}
private void AnalyzeWhile(WhileStatementSyntax node)
{
    var startLine = node.SyntaxTree.GetLineSpan(node.Span).StartLinePosition.Line;
    var endLine = node.SyntaxTree.GetLineSpan(node.Span).EndLinePosition.Line;

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
            SynSet token = WordNetEngine.GetMostCommonSynSet(word, pOS);
            if (token == null)
            {
                throw new Exception("FieldName Is Not A Valid noun");
            }
        }
    }
    if (pOS == WordNetEngine.POS.Verb)
    {
        var word = string.Join(" ", words.Where<string>(c => c.Length > 0));
        SynSet token = WordNetEngine.GetMostCommonSynSet(word, pOS);
        if (token == null)
        {
            throw new Exception("MethodName Is Not A Valid Verb");
        }
    }

}
private void Refresh()
{
    string strand = Newtonsoft.Json.JsonConvert.SerializeObject(ErrorList);
    dynamic dynamic = JsonConvert.DeserializeObject(strand);
    dataGridView.DataSource = dynamic;
}


    }
}
