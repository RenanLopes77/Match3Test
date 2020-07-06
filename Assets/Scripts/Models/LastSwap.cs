public class LastSwap {
  public Cell cellOne;
  public Cell cellTwo;
  public DirectionEnum direction;

  public LastSwap(DirectionEnum direction, Cell cellOne, Cell cellTwo) {
    this.cellOne = cellOne;
    this.cellTwo = cellTwo;
    this.direction = direction;
  }
}