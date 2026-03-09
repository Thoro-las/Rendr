open System
open System.Text
open System.IO

open Rendr.Render
open Rendr.Utils
open Rendr.Colors

let width = Console.WindowWidth
let height = 2 * Console.WindowHeight
let screen: Display = Display(width, height, 1)

let mutable xpos, ypos, zpos = 0, 0, 0
let mutable t = float 0
Console.CursorVisible <- false

let mutable tickTime = DateTime.Now
let mutable frameTime = DateTime.Now
let mutable displayTime = DateTime.Now

let tps = 30
let fps = 60

let debugger =
  let fs =
    new FileStream(
      "/tmp/rendrlog",
      FileMode.Append,
      FileAccess.Write,
      FileShare.Write
    )

  let w = new StreamWriter(fs, Encoding.UTF8)
  w.AutoFlush <- true
  w

let update () =
  if Console.KeyAvailable then
    let key = Console.ReadKey true

    match key.Key with
    | ConsoleKey.RightArrow -> xpos <- (xpos + 1) % width
    | ConsoleKey.LeftArrow -> xpos <- (xpos - 1) % width
    | ConsoleKey.DownArrow -> ypos <- (ypos + 1) % height
    | ConsoleKey.UpArrow -> ypos <- (ypos - 1) % height
    | ConsoleKey.S -> zpos <- zpos - 1
    | ConsoleKey.Z -> zpos <- zpos + 1
    | _ -> ignore ()

let render (screen: Display) =
  t <- t + 0.1
  // let points = 
  //     [ 
  //       Point3D(xpos, ypos, float zpos / 100.0);
  //       Point3D(float(xpos + 10), ypos, float zpos / 100.0);
  //       Point3D(float(xpos + 10), float (ypos + 10), float zpos / 100.0);
  //       Point3D(xpos, float (ypos + 10), float zpos / 100.0);
  //     ] |> List.map Projector.projectToCamera
  // screen.Polygon points (Flat Color.Blue)

  let b = float (abs (4 - ypos))
  let cx, cy = float width / 2.0, float height / 2.0
  let r1 = float 20
  let r2 = float 8
  //
  let pos t r1 r2 =
    Point2D(r1 * cos (float t), r2 * sin (float t))
  //
  screen.Polygon
    [ for i in [ 0 .. int xpos ] do
        Point2D(cx, cy + b)
        + pos (t + float i * 2.0 * Math.PI / float xpos) r1 r2 ]
    (Gradient(Color.White, Color.Red))
  //
  for i in [ 0 .. int xpos ] do
    let p1 =
      Point2D(cx, cy + b) + pos (t + float i * 2.0 * Math.PI / float xpos) r1 r2
  //
    let p2 =
      Point2D(cx, cy - b)
      + pos (t + float (i + 3) * 2.0 * Math.PI / float xpos) r1 r2

    screen.Line p1 p2 (Flat Color.Blue)

  screen.Polygon
    [ for i in [ -1 .. int xpos ] do
        Point2D(cx, cy - b)
        + pos (t + float i * 2.0 * Math.PI / float xpos) r1 r2 ]
    (Gradient(Color.Red, Color.Green))


[<EntryPoint>]
let main _ =
  printf "\u001B[?1049h\u001B[J"

  while true do
    if (DateTime.Now - tickTime).TotalMilliseconds > 16 then
      Console.SetCursorPosition(0, 0)
      screen.Clear Color.Black

      update ()
      render screen

      Console.Out.Write(screen.Show())
      tickTime <- DateTime.Now

  0
