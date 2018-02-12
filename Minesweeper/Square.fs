module Square 

open System

let rand = let rand = new Random() in rand.NextDouble

type SquareState = 
 | SELECTED
 | NEW

type Square = {indx: int; isBomb: bool; mutable state: SquareState; neighborIndx: seq<int>; bombIndx: seq<int>} with         
    member this.weight = this.bombIndx |> Seq.length

// maps 2d points to 1d
let neighbors (width,height) (pos:int) = seq {
  let range x bound = [max 0 (x-1) .. min bound (x+1)] 
  for x in range (pos%width) (width-1) do
    for y in range (pos/width) (height-1) do
      let p = x + y * width
      if not (p = pos) then yield p }

let createSquares height width =
    let bombs = Array.init (height*width) (fun _ -> rand() < 0.2)
    let squares =
        Array.init (height*width) (fun i ->             
            let neighbors = neighbors(width, height) i
            let neighboringBombs = neighbors |> Seq.filter(fun p -> bombs.[p])                                    
            {indx = i; isBomb = bombs.[i]; state = NEW; neighborIndx = neighbors; bombIndx = neighboringBombs})                                  
    squares                   


