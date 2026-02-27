namespace Utils
open System

type Point2D(x: float, y: float) =
  member _.x = x
  member _.y = y

  static member (*)(scalar: float, p: Point2D) =
    Point2D(scalar * p.x, scalar * p.y)

  static member (+)(p1: Point2D, p2: Point2D) =
    Point2D(p1.x + p2.x, p1.y + p2.y)

  static member (-)(p1: Point2D, p2: Point2D) =
    Point2D(p1.x - p2.x, p1.y - p2.y)

  static member Distance(p1: Point2D, p2: Point2D) =
    Math.Pow(p1.x - p2.x, 2) +
    Math.Pow(p1.y - p2.y, 2)
      |> Math.Sqrt


type Point3D(x: float, y: float, z: float) =
  member _.x = x
  member _.y = y
  member _.z = z

  static member (*)(scalar: float, p: Point3D) =
    Point3D(scalar * p.x, scalar * p.y, scalar * p.z)

  static member (+)(p1: Point3D, p2: Point3D) =
    Point3D(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z)

  static member (-)(p1: Point3D, p2: Point3D) =
    Point3D(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z)

  static member Distance(p1: Point3D, p2: Point3D) =
    Math.Pow(p1.x - p2.x, 2) + 
    Math.Pow(p1.y - p2.y, 2) +
    Math.Pow(p1.z - p2.z, 2)
      |> Math.Sqrt
