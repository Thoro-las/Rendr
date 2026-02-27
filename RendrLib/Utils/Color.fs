namespace Render


type Color = { r: byte; g: byte; b: byte; a: byte }

module Color =
  let fromHEX (hex: int) : Color =
    { r = byte (hex &&& 0xFF)
      g = byte (hex >>> 8 &&& 0xFF)
      b = byte (hex >>> 16 &&& 0xFF)
      a = byte (hex >>> 24 &&& 0xFF) }

  let toANSI (color: Color) : string =
    sprintf "%d;%d;%d" color.r color.g color.b

  let Black = fromHEX 0xFF000000
  let Gray = fromHEX 0xFF999999
  let White = fromHEX 0xFFFFFFFF

  let Red = fromHEX 0xFFFF0000
  let Green = fromHEX 0xFF00FF00
  let Blue = fromHEX 0xFF0000FF

  let Transparent = fromHEX 0x00000000
