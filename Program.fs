open System
open Render

let width = Console.WindowWidth
let height = 2 * Console.WindowHeight
let screen: Display = Display(width, height)

let mutable xpos, ypos = 1, 0
let mutable t = float 0
Console.CursorVisible <- false

let mutable tickTime = DateTime.Now

[<EntryPoint>]
let main _ =
  while true do
    t <- t + 0.0000030

    if Console.KeyAvailable then
      let key = Console.ReadKey true

      match key.Key with
      | ConsoleKey.RightArrow -> xpos <- (xpos + 1) % width
      | ConsoleKey.LeftArrow -> xpos <- (xpos - 1) % width
      | ConsoleKey.DownArrow -> ypos <- (ypos + 1) % height
      | ConsoleKey.UpArrow -> ypos <- (ypos - 1) % height
      | _ -> ignore ()

    if (DateTime.Now - tickTime).TotalMilliseconds > 16 then
      printf "\u001B[J\u001B[H"
      screen.Clear Color.Black

      let b = abs (4 - ypos)
      let cx, cy = width / 2, height
      let r1 = float 20
      let r2 = float 4

      let pos t r1 r2 =
        int (r1 * cos (float t)),
        int (r2 * sin (float t))

      let add p1 p2 = fst p1 + fst p2, snd p1 + snd p2


      screen.Polygon [
        for i in [0..int xpos] do
          add (cx, cy + b) (pos (t + float i * 2.0 * Math.PI / float xpos) r1 ((3.0 - float ypos) * r2))
      ] Color.White

      for i in [0..int xpos] do
        let p1 = add (cx, cy + b) (pos (t + float i * 2.0 * Math.PI / float xpos) r1 ((3.0 - float ypos) * r2))
        let p2 = add (cx, cy - b) (pos (t + float i * 2.0 * Math.PI / float xpos) r1 ((3.0 - float ypos) * r2))
        screen.Line p1 p2 Color.Gray

      screen.Polygon [
        for i in [-1..int xpos] do
          add (cx, cy - b) (pos (t + float i * 2.0 * Math.PI / float xpos) r1 ((3.0 - float ypos) * r2))
      ] Color.White

      screen.Point (int xpos, int ypos) Color.Blue
      printf "%s" (screen.Show())
      tickTime <- DateTime.Now

  0
