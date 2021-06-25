export interface IMazeCell {
  symbol: string;
  row: number;
  col: number;
  north: boolean;
  west: boolean;
  east: boolean;
  south: boolean;
}
