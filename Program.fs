open System
open Render
open Utils

let width = Console.WindowWidth
let height = 2 * Console.WindowHeight
let screen: Display = Display(width, height)

let mutable xpos, ypos = 1, 0
let mutable t = float 0
Console.CursorVisible <- false

let mutable tickTime = DateTime.Now

let img = Image.loadPNG "Resources/img.png"

[<EntryPoint>]
let main _ =
  printf "\u001B[?1049h\u001B[J"

  while true do
    t <- t + 0.00001

    if Console.KeyAvailable then
      let key = Console.ReadKey true

      match key.Key with
      | ConsoleKey.RightArrow -> xpos <- (xpos + 1) % width
      | ConsoleKey.LeftArrow -> xpos <- (xpos - 1) % width
      | ConsoleKey.DownArrow -> ypos <- (ypos + 1) % height
      | ConsoleKey.UpArrow -> ypos <- (ypos - 1) % height
      | _ -> ignore ()

    if (DateTime.Now - tickTime).TotalMilliseconds > 16 then
      Console.SetCursorPosition(0, 0)
      screen.Clear Color.Black

      screen.Image img (Point2D(0, 0))

      // let b = float (abs (4 - ypos))
      // let cx, cy = float width / 2.0, float height / 2.0
      // let r1 = float 20
      // let r2 = float 8
      //
      // let pos t r1 r2 =
      //   Point2D(r1 * cos (float t), r2 * sin (float t))
      //
      //
      // screen.Polygon [
      //   for i in [0..int xpos] do
      //     Point2D(cx, cy + b) + pos (t + float i * 2.0 * Math.PI / float xpos) r1 r2
      // ] Color.White
      //
      // for i in [0..int xpos] do
      //   let p1 = Point2D(cx, cy + b) + pos (t + float i * 2.0 * Math.PI / float xpos) r1 r2
      //   let p2 = Point2D(cx, cy - b) + pos (t + float (i + 3) * 2.0 * Math.PI / float xpos) r1 r2
      //   screen.Line p1 p2 Color.Gray
      //
      // screen.Polygon [
      //   for i in [-1..int xpos] do
      //     Point2D(cx, cy - b) + pos (t + float i * 2.0 * Math.PI / float xpos) r1 r2
      // ] Color.White

      printf "%s" (screen.Show())
      tickTime <- DateTime.Now

  0
