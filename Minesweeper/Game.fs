module Game

open System
open System.Drawing
open System.Windows.Input
open System.Windows.Forms
open System.Drawing.Drawing2D
open Board

let gameUI() =    
    // constants
    let Width = 3
    let Height = 3
    let SquareDim = 30
    let SquareSize = new Size(SquareDim, SquareDim)            
    let NewButtonColor = Color.Gray
    let SelectedButtonColor = Color.White
    let ButtonTextColor = Color.Red
    
    let form = new Form(Text = "Minesweeper")
    let board = new Board(Height, Width)
                       
    let gameButtons = 
        [for y in 1..Height do for x in 1..Width -> 
           new Button(
                Size=SquareSize, 
                Location=Point((x*SquareSize.Width), (y*SquareSize.Height)), 
                BackColor= NewButtonColor)]       
                                                                              
    let getIndx(button: Button) =                
        ((button.Location.Y / SquareDim - 1) * Width) + button.Location.X / SquareDim - 1           
                                               
    let openDialog(msg: String) =        
        match MessageBox.Show(msg, "", MessageBoxButtons.YesNo) with
        |DialogResult.Yes -> Application.Restart() // handle without restarting
        |DialogResult.Cancel -> ()
        |_ -> Application.Exit()                                               
                               
    let updateButton(indx: seq<int>)=        
        for i in indx do        
            let button = gameButtons.[i]
            button.Text <- board.getSquareWeight i
            button.BackColor <- ButtonTextColor
            button.BackColor <- SelectedButtonColor
                                                        
    let onClick(button: Button, indx: int) =       
       if button.Enabled then                         
           match board.updateGameStatus indx with    
           | WIN -> openDialog("You Won! Do you want to play again?")               
           | NEXT -> updateButton(board.getSelectSquaresIndx())                
           | LOST ->             
                updateButton(board.getMinesIndx())
                openDialog("You hit a mine. Do you want to play again?")
       button.Enabled <- false
        
    let addButtonControl(button: Button) = 
        button.Click.Add(fun _ ->  
            let indx = getIndx button
            onClick(button, indx))        
                                                                                 
    gameButtons |> Seq.iter(fun b -> addButtonControl b)
    form.Controls.AddRange(gameButtons |> Seq.cast |> Seq.toArray)                                                         
    form
                      
[<EntryPoint>]
let main _ =        
  Application.Run(gameUI())
  0


