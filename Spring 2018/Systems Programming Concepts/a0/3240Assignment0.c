#include<stdio.h>
#include<stdlib.h>
#include<string.h>

typedef struct {
  char * location;
  char * artist;
  char * songName;
  char * albumName;
  float duration;
  int year;
  double hot; } songs;


void Read(int numLines, int lineSize, char delimiter, int item, songs **s);
void Sort(songs **s, int low, int high);
int SortPart(songs **s, int low, int high);
void Swap(songs *a, songs *b);
int Search(songs **s, char *search, int l, int r);
void StartTest(songs **s, int numLines);
void HRule(void);
void SearchMiss(char *search);
void SearchHit(songs **s, int index);


int main(int argc, char * argv[]){ 
  const int numLines = 10002; // num elements for struct array
  const int lineSize = 1024; // upperbound char count per line
  const char delimiter = ',';
  int item = 0; // used to keep track of struct array index

  // array of pointers to songs struct. initialize at 5000 long
  songs *s[numLines];
  
  // call func to read in all items from csv
  Read(numLines, lineSize, delimiter, item, s);

  // func to sort songs struct
  Sort(s, 0, 10000);
  
  // func to do test searches
  StartTest(s, numLines);
}

void StartTest(songs **s, int numLines){
  char tbuf[500]; // buffer size for song name
  char *tline = fgets(tbuf, 499, stdin); // save line of test file
  if(tline == NULL){exit(-1);}
  int sr = -1; // initialize search result index as -1
  
  while(tbuf != 0){
    tline = strtok(tline, "\n"); // remove newline char
    if(tline != NULL){
      if(strcmp(tline, "ZZZ") == 0){ // checks for exit code from user
        printf("!!! EXITING !!!\n"); 
        exit(0);
      }
      sr = Search(s, tline, 0, numLines); // saves search result index
      if(sr < 0){SearchMiss(tline);} // search returns -1 on miss
      else{SearchHit(s, sr);} // hit
    }
    tline = fgets(tbuf, 499, stdin); // read next line from stdin
    if(tline == NULL){exit(-1);} 
    HRule(); // simple horizonal rule print function
  }
}

// Horizontal rule used often enough to warrant function
void HRule(void){printf("--------------------\n");}

// When search misses. print error
void SearchMiss(char *search){printf("\"%s\" NOT FOUND\n", search);}

// On search hit, output struct information at index
void SearchHit(songs **s, int index){
  printf("SEARCH RESULT\n"
         " ============= \n" 
         " Song Name: %s \n"
         " Artist:    %s \n"
         " Album:     %s \n"
         " Location:  %s \n"
         " Duration:  %f \n"
         " Hotness:   %f \n"
         " Year:      %d \n",
          s[index]->songName, 
          s[index]->artist,
          s[index]->albumName, 
          s[index]->location,
          s[index]->duration, 
          s[index]->hot, 
          s[index]->year);
}

// Standard binary search by songName
int Search(songs **s, char *search, int l, int r){
  if(r >= l){
    int m = l + (r-l)/2;
    int sr = strcmp(search, s[m]->songName);
    if(sr == 0){ // if search hit
      return m;
    }
    else if(sr < 0){ // miss, move left
      return Search(s, search, l, m-1); 
    }
    else{ // miss, move right
      return Search(s, search, m+1, r);
    }
  }
  return -1; // indicates complete miss
}

// Standard quicksort
void Sort(songs **s, int low, int high){
  if(low < high){
    int pi = SortPart(s, low, high); // pivot index
    Sort(s, low, pi - 1); // recursive call sort moving pivot index left
    Sort(s, pi + 1, high); // rec call moving pivot index right
  }
}
int SortPart(songs **s, int low, int high){ // partition method used by Sort
  char *pivot = s[high]->songName; // pivot compare value
  int i = low - 1;
  for(int j = low; j <= high - 1; j++){
    if(strcmp(s[j]->songName, pivot) <= 0){ // compare current sontName to pivot
      i++;
      Swap(s[i],s[j]); // swap pointers and index i and j
    }
  }
  Swap(s[i+1], s[high]);
  return i + 1;
}
void Swap(songs *a, songs *b){ // swap utility func used by SortPart
  songs t = *a; // temp pointer while a gets overwritten
  *a = *b;
  *b = t;
}

// Reads csvfile line by line then saves appropriate fields into struct
void Read(int numLines, int lineSize, char delimiter, int item, songs **s){
  FILE * inputFile = fopen("./SongCSV.csv", "r");
  // buffer to store read lines
  char buf[lineSize];
  // read in first line. This will be the header
  char *line = fgets(buf, lineSize - 1, inputFile);
  if(line == NULL){exit(-1);}
  // char array used to temp store entries to be converted to diff data types.
  char * temp;
  // Allocate mem for songs struct items
  for(int i = 0; i < numLines; i++) { 
    s[i] = malloc(sizeof(songs));
    if(NULL == s[i]){exit(-1);} 
  }
  while (buf != 0){ // while not EOF
    line = fgets(buf, lineSize - 1, inputFile); // read next line
    if(line == NULL) {return;}
    // for each char in line
    for(int i = 0, j = 0, index = 0; i < strlen(line); j++){
      if(line[j] == delimiter){ // check if at comma (or custom delim)
        switch(index){ // checks if at field that needs to be stored
          case 3:
            s[item]->albumName = malloc(j-i+1); // allocate memory for new item
            strncpy(s[item]->albumName, buf + i + 2, j-i-3); // save in struct
            break;
          case 6:
            s[item]->location = malloc(j-i+1);
            strncpy(s[item]->location, buf + i + 2, j-i-3);
            break;
          case 8:
            s[item]->artist = malloc(j-i+1);
            strncpy(s[item]->artist, buf + i + 2, j-i-3);
            break;
          case 10:
            temp = malloc(j-i+1);
            strncpy(temp, buf + i + 1, j-i-1);
            s[item]->duration = atof(temp);
            break;
          case 14:
            strncpy(temp, buf + i + 1, j-i-1);
            s[item]->hot = atof(temp);
            break;
          case 17:
            s[item]->songName = malloc(j-i+1);
            strncpy(s[item]->songName, buf + i + 2, j-i-3);
            break;
          case 18:
            temp = malloc(j-i+1);
            strncpy(temp, buf + i + 1, j-i);
            s[item]->year = atoi(temp);
            break;
          default:
            break;
        }
        index++; // increment index (each comma increments this)
        i = j; // update i index to current comma
      }
    }
    item++; // increment item (associated with index in songs struct array)
  }
}

