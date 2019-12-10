<Query Kind="FSharpProgram" />

let curqp = Util.CurrentQueryPath
let dirname = Path.GetDirectoryName(curqp)
Directory.SetCurrentDirectory(dirname)

let inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt")
let input = File.ReadLines(inputName)

let slash = new Regex(@"\\\\")
let quote = new Regex(@"\\""")
let hex = new Regex(@"\\\\x..")

let len (s:string) =
    s.Length - 2 - slash.Matches(s).Count - quote.Matches(s).Count - 3 * hex.Matches(s).Count

"\\\"s\\\\df\\x34".Length |> Dump

len "\"\\\"s\\\\df\\x34\"" |> Dump
