namespace Rendr.Render

open Rendr.Utils

type Camera = {
  Position: Point3D
  ZNear: float
}

module Projector =
  let projectToCamera (point: Point3D) : Point2D =
    Point2D(point.x / point.z, point.y / point.z)
