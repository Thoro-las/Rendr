namespace Rendr.Utils

open SixLabors.ImageSharp

type Image =
  { Width: int
    Height: int
    Pixels: Rendr.Colors.Color[,] }

module Image =
  open SixLabors.ImageSharp.PixelFormats

  let loadPNG (path: string) : Image =
    let image = Image.Load<Rgba32> path

    let pixels =
      Array2D.init image.Width image.Height (fun x y ->
        let pixel = image.[x, y]

        { r = pixel.R
          g = pixel.G
          b = pixel.B
          a = pixel.A } : Rendr.Colors.Color)

    { Width = image.Width
      Height = image.Height
      Pixels = pixels }
