namespace Rendr.Colors

type Stroke = 
  | Flat of Color
  | Gradient of Color * Color

module Stroke =
  let toColorMap (stroke: Stroke) : float -> Color =
    match stroke with
    | Flat c -> fun _ -> c
    | Gradient (c1, c2) -> 
      fun t -> {
        r = t * float c1.r + (1.0 - t) * float c2.r |> byte
        g = t * float c1.g + (1.0 - t) * float c2.g |> byte
        b = t * float c1.b + (1.0 - t) * float c2.b |> byte
        a = t * float c1.a + (1.0 - t) * float c2.a |> byte
      }
