namespace Render

open System

type Color = { r: byte; g: byte; b: byte }


module Color =
  let fromHEX (hex: string) : Color =
    { r = Convert.ToByte(hex.Substring(1, 2), 16)
      g = Convert.ToByte(hex.Substring(3, 2), 16)
      b = Convert.ToByte(hex.Substring(5, 2), 16) }

  let toANSI (color: Color) : string =
    sprintf "%d;%d;%d" color.r color.g color.b


  let Black = fromHEX "#000000"
  let Gray = fromHEX "#999999"
  let White = fromHEX "#FFFFFF"

  let Red = fromHEX "#FF0000"
  let Green = fromHEX "#00FF00"
  let Blue = fromHEX "#0000FF"
