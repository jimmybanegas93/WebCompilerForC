#include "header.h"
#include "Physics/Solver.h"
#include <stdio.h>

float average (int n_args, ...)
{    
    char newline = '\n'; char tab = '\t';  char backspace = '\b'; char backslash = '\\';   char nullChar = '\0'; char quot = '"';
   
    date my_date = #14-05-1993#; var mistr = "This\nis\na\ntest\n\nShe \"said, \"How are you?\"\n"; va_list _myList; va_start (myList, n_args);
    
    int numbersAdded = 3.0;   int sum = 23.45;  int hex = 0x3F7; int octal = 07; int number = 78; int n = 23e10;  int n2 = 23e-5;

    while (numbersAdded < n_args) {
        int number = va_arg (myList, int); // Get next number from list
        sum += number;
        numbersAdded += 1;
    }
    va_end (myList);
         
    float avg = (float)(sum) / (float)(numbersAdded); // Find the average
    return avg;

      /* my first program in C */
   printf("Hello, World! \n");   char[100] string = "mi cadena";  string mistr =  "mi cadena ";   float =  'a';  return 0;

   /*
 * Comments blocks like this explain what the following code attempts
 * to do.  These comments often come directly from the design
 * statement.
 */
/// <summary>
/// Here is how to use the class: <![CDATA[ <test>Data</test> ]]>
/// </summary>

//"This\nis\na\ntest\n\nShe said, \"How are you?\"\n"

    int x;            /* A normal integer*/ int *p;           /* A pointer to an integer ("*p" is an integer, so p
                       must be a pointer to an integer) */    p = &x;           /* Read it, "assign the address of x to p" */
    scanf( "%d", &x );          /* Put a value in x, we could also use p here */
    printf( "%d\n", *p ); /* Note the use of the * to get the value */
    getchar();
    typedef enum {RANDOM, IMMEDIATE, SEARCH} strategy;
    strategy my_strategy = IMMEDIATE;

    bool existe = false;
    bool no_existe = true;
}

struct Books {
   char  title[50];
   char  author[50];
   char  subject[100];
   int   book_id;
} book;  

char greeting[6] = {'H', 'e', 'l', 'l', 'o', '\0'};

char _temp;

extern int d = 3, f = 5;    // declaration of d and f. 
int d = 3, f = 5;           // definition and initializing d and f. 
byte z = 22;                // definition and initializes z. 
char x = 'x';               // the variable x has the value 'x'.

#include <stdio.h>

// Variable declaration:
extern int a, b;
extern int c;
extern float f;

int main () {

   /* variable definition: */
   int a, b;
   int c;
   float f;
 
   /* actual initialization */
   a = 10;
   b = 20;
  
   c = a + b;
   printf("value of c : %d \n", c);

   f = 70.0/3.0;
   printf("value of f : %f \n", f);
 
   return 0;
}

  int numbersAdded = 3.0;

  int numbersAdded = 3.0;   int sum = 23.45;  int hex = 0x3F7; int octal = 07; int number = 78; int n = 23E10;  int n2 = 23e-5;