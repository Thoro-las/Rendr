namespace Rendr.Render

type Pixel = Color

type Buffer (width: int, height: int) =
  member _.Width = width
  member _.Height = height

  member val Pixels: Pixel list = [] with get, set
  member val Ready = false
