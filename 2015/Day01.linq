<Query Kind="FSharpProgram" />

let curqp = Util.CurrentQueryPath
let dirname = Path.GetDirectoryName(curqp)
Directory.SetCurrentDirectory(dirname)

let inputName = Path.Combine(dirname,Path.GetFileNameWithoutExtension(curqp) + ".txt")

let input = File.ReadLines(inputName)

let santasElevator (i: int, p1: int, p2: int option) c =
    let p1' = p1 + match c with '(' -> 1 | ')' -> -1
    let p2' =
        if p1' >= 0 || p2.IsSome then
            p2
        else
            Some(i)
    
    (i+1,p1',p2')
        
    
input |> Seq.head |> Seq.fold santasElevator (1,0,None) |> (fun (_,p1,Some(p2)) -> printfn "Part 1: %i\tPart 2: %i" p1 p2)