namespace Rendr.Render

open Rendr.Colors

type Pixel = Color

type Buffer(width: int, height: int) =
  member _.Width = width
  member _.Height = height

  member val Pixels: Pixel array =
    Array.init (width * height) (fun _ -> Color.Transparent) with get, set
