<Query Kind="FSharpProgram" />

let curqp = Util.CurrentQueryPath
let dirname = Path.GetDirectoryName(curqp)
Directory.SetCurrentDirectory(dirname)

let inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt")

let input = File.ReadLines(inputName)

let paperForDimensions l w h =
    [| l*w; l*h; w*h |] |> Array.sort |> (fun xs -> xs.[0] * 3 + xs.[1] * 2 + xs.[2] * 2)
    
let ribbonForDimensions l w h =
    [| l; w; h |] |> Array.sort |> (fun xs -> xs.[0] * 2 + xs.[1] * 2 + l * w * h)

let dimensions = input |> Seq.map (fun s -> s.Split('x') |> Seq.toList |> List.map int)

dimensions |> Seq.map (fun p -> match p with l::w::h::[] -> paperForDimensions l w h) |> Seq.sum |> Dump |> ignore

dimensions |> Seq.map (fun p -> match p with l::w::h::[] -> ribbonForDimensions l w h) |> Seq.sum |> Dump |> ignore