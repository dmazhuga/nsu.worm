namespace NSU.Worm.Data
{
    public class FoodGenerationPatternStep
    {
        public int Id { get; set; }
        
        public FoodGenerationPattern Pattern { get; set; }
        
        public int Iteration { get; set; }
        
        public int PositionX { get; set; }
        
        public int PositionY { get; set; }
    }
}