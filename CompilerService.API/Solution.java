import java.util.*;public class Solution  {public static void main(String[] args) {Solution s= new Solution();s.TestCase();}public static void assertEqual(Object expected, Object actual) {if (expected == null && actual == null) {return;}if (expected == actual ) {return;}if (expected == null || !expected.equals(actual)) {System.out.println("Test Failed:Expected: " + expected + ", but was: " + actual);}}public  int Answer(int a, int b) {return a+b;}public void TestCase() {assertEqual(Answer(5,4),9);assertEqual(Answer(3,4),7);assertEqual(Answer(2,4),6);assertEqual(Answer(5,9),14);}}