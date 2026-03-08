namespace Rendr.Render

open Rendr.Colors
open Rendr.Utils

open System.Text

type Display(width: int, height: int) =
  member _.Width = width
  member _.Height = height
  member val Pixels: Pixel array = Array.empty with get, set

  member val private buffer = StringBuilder(6 * width * height)

  member private this.InBounds (p: Point2D) =
    p.x >= 0 && p.x < this.Width && p.y >= 0 && p.y < this.Height

  member private this.SetPixel (p: Point2D) (color: Color) =
    if this.InBounds p && color <> Color.Transparent then
      this.Pixels.[int p.y * this.Width + int p.x] <- color

  member this.Point (p: Point2D) (color: Color) = this.SetPixel p color

  member this.Line (a: Point2D) (b: Point2D) (stroke: Stroke) =
    let n = max (abs (a.x - b.x)) (abs (a.y - b.y)) |> ceil |> int
    let cm = Stroke.toColorMap stroke

    if n <> 0 then
      let interpolate (p1: Point2D) (p2: Point2D) (l: float) =
        Point2D(p1.x + (p2.x - p1.x) * l, p1.y + (p2.y - p1.y) * l)

      for i in [ 0..n ] do
        let t = float i / float n
        this.SetPixel (interpolate a b t) (cm t)

  member this.Polygon (points: Point2D list) (stroke: Stroke) =
    let n = List.length points

    [ 0 .. n - 2 ]
    |> List.map (fun i -> points.[i], points.[i + 1])
    |> List.append [ (points.[n - 1], points.[0]) ]
    |> List.iter (fun ps -> this.Line (fst ps) (snd ps) stroke)

  member this.Image (image: Image) (position: Point2D) =
    for x in [ 0 .. image.Width - 1 ] do
      for y in [ 0 .. image.Height - 1 ] do
        this.SetPixel (position + Point2D(x, y)) image.Pixels.[x, y]

  member this.Clear(color: Color) =
    for x in [ 0 .. this.Width - 1 ] do
      for y in [ 0 .. this.Height - 1 ] do
        this.SetPixel (Point2D(x, y)) color

  member this.Show() =
    this.buffer.Clear() |> ignore

    for j in [ 0 .. (this.Height - 1) / 2 ] do
      for i in [ 0 .. this.Width - 1 ] do
        let ct = this.Pixels.[i + 2 * j * this.Width] 
        let cb = this.Pixels.[i + (2 * j + 1) * this.Width]

        this.buffer
          .Append("\u001B[38;2;")
          .Append(Color.toANSI ct)
          .Append("m\u001B[48;2;")
          .Append(Color.toANSI cb)
          .Append("m▀")
        |> ignore

      if j < (this.Height - 1) / 2 then
        this.buffer.Append "\n" |> ignore

    this.buffer.ToString()
