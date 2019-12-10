<Query Kind="FSharpProgram" />

let curqp = Util.CurrentQueryPath
let dirname = Path.GetDirectoryName(curqp)
Directory.SetCurrentDirectory(dirname)

let inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt")
let input = File.ReadLines(inputName)

let rx = new Regex("(?<command>turn on|turn off|toggle) (?<x0>\d+),(?<y0>\d+) through (?<x1>\d+),(?<y1>\d+)")

let grid = Array2D.create 1000 1000 false
let grid2 = Array2D.create 1000 1000 0

type Command = On | Off | Toggle

let parseCommand s =
    let m = rx.Match(s)
    let cmd = match m.Groups.["command"].Value with
              | "turn on" -> On
              | "turn off" -> Off
              | "toggle" -> Toggle
              | _ -> failwith "Failed to parse problem set."

    let x0, y0 = int m.Groups.["x0"].Value, int m.Groups.["y0"].Value
    let x1, y1 = int m.Groups.["x1"].Value, int m.Groups.["y1"].Value
    
    ((x0, y0), (x1, y1), cmd)

let commands = input |> Seq.map parseCommand

for ((x0, y0), (x1, y1), cmd) in commands do
    for x = x0 to x1 do
        for y = y0 to y1 do
            grid.[x,y] <-  match cmd with
                           | On -> true
                           | Off -> false
                           | Toggle -> not grid.[x,y]
            grid2.[x,y] <- match cmd with
                           | On -> grid2.[x,y] + 1
                           | Off -> max 0 (grid2.[x,y] - 1)
                           | Toggle -> grid2.[x,y] + 2

grid |> Seq.cast<bool> |> Seq.fold (fun c b -> if b then c + 1 else c) 0 |> Dump |> ignore
grid2 |> Seq.cast<int> |> Seq.fold (fun c b -> c + b) 0 |> Dump |> ignore