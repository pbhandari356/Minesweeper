module Board

open Square

type GameStatus = 
| WIN
| LOST
| NEXT

type Board(height: int, width: int) = 
    let squares = Square.createSquares height width
                                
    let updateBoard(indx: int) =        
        let rec selectSquare i =            
            let s = squares.[i]            
            if s.state = NEW && not s.isBomb then
                s.state <- SELECTED
                if s.weight = 0 then s.neighborIndx |> Seq.iter(fun i -> selectSquare(i))                
        selectSquare indx        

    member this.updateGameStatus indx =       
       let square = squares.[indx]
       match square.isBomb with
       |true -> LOST
       |false ->         
         updateBoard(indx)                  
         if squares |> Seq.forall(fun s -> s.state <> NEW || s.isBomb) then    
            WIN
         else 
            NEXT   
            
    member this.getSelectSquaresIndx() = 
        squares |> Seq.filter(fun s -> s.state <> NEW) |> Seq.map(fun s -> s.indx)
        
    member this.getMinesIndx() =         
       squares |> Seq.filter(fun s -> s.isBomb) |> Seq.map(fun s -> s.indx)
       
    member this.getSquareWeight indx = 
       let s = squares.[indx]
       if s.isBomb then "*" else string(s.weight)