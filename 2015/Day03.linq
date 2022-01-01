<Query Kind="FSharpProgram" />

let curqp = Util.CurrentQueryPath
let dirname = Path.GetDirectoryName(curqp)
Directory.SetCurrentDirectory(dirname)

let inputName = Path.Combine(dirname, Path.GetFileNameWithoutExtension(curqp) + ".txt")
let input = File.ReadLines(inputName)

let directions = input.First()

type Loc = int * int
type Locs = Set<Loc>

let countLocs ((x,y),locs:Locs) ch = 
    let (dx, dy) =
        match ch with
            | 'v' -> ( 0,  1)
            | '^' -> ( 0, -1)
            | '<' -> (-1,  0)
            | '>' -> ( 1,  0)
    
    let newloc = (x+dx,y+dy)
    
    (newloc, locs.Add(newloc))
    
directions |> Seq.fold countLocs ((0,0), Set.empty.Add((0,0))) |> fun (_,locs) -> locs.Count |> Dump

let rec countLocsWithRobo (sloc,sset:Locs) (rloc,rset:Locs) chs =
    match chs with
        | [] -> (sset + rset).Count
        | sch::rch::rest -> countLocsWithRobo (countLocs (sloc,sset) sch) (countLocs (rloc,rset) rch) rest

directions |> Seq.toList |> countLocsWithRobo ((0,0), Set.empty.Add((0,0))) ((0,0), Set.empty.Add((0,0))) |> Dump