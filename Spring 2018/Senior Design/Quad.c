#include<stdio.h>
#include<stdlib.h>
#include<string.h>
#include<math.h>

int main(int argc, char *argv[])
{
  // inputs a b and c are passed in as arguments
  double a = atof(argv[1]), b = atof(argv[2]), c = atof(argv[3]);

  // find discriminant
  double dis = (b*b)-4*(a*c);

  // Output the values entered in in quadratic form
  printf("------------------------------------------------------\n");
  printf("Quadratic Equation:\n  %0.32fx^2\n+ %0.32fx\n+ %0.32f\n= 0\n", a, b, c);

  // if discriminant = 0, one solution, if > 0, 2 solutions, if < 0, no real solutions.
  if(dis == 0)
  {
    printf("RESULTS:\n x = %0.32f\n", (-1 * b + sqrt(dis))/(2 * a));
  } // One real root
  else if(dis > 0)
  {
    printf("RESULTS:\nx = %0.32f\nx = %0.32f\n", (-1 * b + sqrt(dis))/(2 * a), (-1 * b - sqrt(dis))/(2 * a));
  } // Two real roots
  else if(dis < 0)
  {
    printf("Irrational Results\n");
  } // Two complex roots
  else
  {
    printf("shouldnt be here\n");
  } // Indicates bad value for dis. Shouldn't ever reach this statement.

  return 0;
}
