namespace CircuitYard.Server.Models;

public class Chunk
{
    public int X { get; set; }
    public int Y { get; set; }
    public List<Cell> Cells { get; set; } = [];
}
