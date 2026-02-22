namespace Render
open System.Text

type Point = int * int

type Display(width: int, height: int, pixels: Color[,]) =
  new(width, height) =
    Display(width, height, Array2D.create width height Color.Black)

  member _.Width = width
  member _.Height = height
  member val Pixels = pixels with get, set

  member private this.InBounds(p: Point) =
    fst p >= 0 && fst p < this.Width && snd p >= 0 && snd p < this.Height

  member private this.SetPixel (p: Point) (color: Color) =
    if this.InBounds p then
      this.Pixels.[fst p, snd p] <- color

  member this.Point (p: Point) (color: Color) = this.SetPixel p color

  member this.Line (a: Point) (b: Point) (color: Color) =
    let n = max (abs (fst a - fst b)) (abs (snd a - snd b))

    if n <> 0 then
      let interpolate (x1, y1) (x2, y2) (l: float) =
        int (float x1 + float (x2 - x1) * l),
        int (float y1 + float (y2 - y1) * l)

      for i in [ 0..n ] do
        this.SetPixel (interpolate a b (float i / float n)) color

  member this.Polygon (points: Point list) (color: Color) =
    let n = List.length points

    [ 0 .. n - 2 ]
    |> List.map (fun i -> points.[i], points.[i + 1])
    |> List.append [ (points.[n - 1], points.[0]) ]
    |> List.iter (fun ps -> this.Line (fst ps) (snd ps) color)

  member this.Clear(color: Color) =
    for x in [ 0 .. this.Width - 1 ] do
      for y in [ 0 .. this.Height - 1 ] do
        this.SetPixel (x, y) color

  member this.Show() =
    let buffer = StringBuilder()
    for j in [0.. (this.Height - 1) / 2] do
      for i in [0.. (this.Width -1)] do
        buffer.Append "\u001B[32;2;"
        if 2*j + 1 > this.Height
          then 
        // let top = this.Pixels.[i, 2*j]
        // let bottom = 
        //   if 

    [ 0 .. (this.Height - 1) / 2 ]
    |> Seq.map (fun j ->
      [ 0 .. this.Width - 1 ]
      |> Seq.map (fun i ->
        sprintf
          "\u001B[38;2;%sm\u001B[48;2;%sm▀\u001B[0m"
          (Color.toANSI this.Pixels.[i, 2 * j])
          (if 2 * j + 1 >= this.Height then
             Color.toANSI Color.Red
           else
             Color.toANSI this.Pixels.[i, 2 * j + 1]))
      |> String.concat "")
    |> String.concat "\n"
