namespace Render

open Utils
open System.Text

type Pixel =
  | Color of Color

type Display(width: int, height: int, pixels: Pixel[,]) =
  new(width, height) =
    Display(width, height, Array2D.create width height (Color Color.Black))

  member _.Width = width
  member _.Height = height
  member val Pixels = pixels with get, set

  member val private buffer = StringBuilder(6 * width * height)

  member private this.InBounds(p: Point2D) =
    p.x >= 0 && p.x < this.Width && p.y >= 0 && p.y < this.Height

  member private this.SetPixel (p: Point2D) (color: Color) =
    if this.InBounds p then
      this.Pixels.[p.x |> int, p.y |> int] <- Color color

  member this.Point (p: Point2D) (color: Color) = this.SetPixel p color

  member this.Line (a: Point2D) (b: Point2D) (color: Color) =
    let n = max (abs (a.x - b.x)) (abs (a.y - b.y)) |> ceil |> int

    if n <> 0 then
      let interpolate (p1: Point2D) (p2: Point2D) (l: float) =
        Point2D(p1.x + (p2.x - p1.x) * l, p1.y + (p2.y - p1.y) * l)

      for i in [ 0..n ] do
        this.SetPixel (interpolate a b (float i / float n)) color

  member this.Polygon (points: Point2D list) (color: Color) =
    let n = List.length points

    [ 0 .. n - 2 ]
    |> List.map (fun i -> points.[i], points.[i + 1])
    |> List.append [ (points.[n - 1], points.[0]) ]
    |> List.iter (fun ps -> this.Line (fst ps) (snd ps) color)

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
        match this.Pixels.[i, 2 * j] with
        | Color ct ->
          match this.Pixels.[i, 2 * j + 1] with
          | Color cb ->
            this.buffer
              .Append("\u001B[38;2;")
              .Append(Color.toANSI ct)
              .Append("m\u001B[48;2;")
              .Append(Color.toANSI cb)
              .Append("m▀")
              .Append("\u001B[0m")
            |> ignore

      if j < (this.Height - 1) / 2 then
        this.buffer.Append "\n" |> ignore

    this.buffer.ToString()
