namespace Tests;

public class UnitTest1
{
   [Fact]
       public void Test1()
       {
           Assert.Equal(4, Add(2, 2));
       }
   
       [Fact]
       public void FailTest()
       {
           Assert.Equal(4, Add(2, 3));
       }
   
       public int Add(int x, int y)
       {
           return x + y;
       }
}