#include<stdio.h>
#include<stdlib.h>

typedef int iii; // Creates new name for existing data type
typedef struct {char * first; char * mid; char * last} name; // Creates standard linked list node type.
struct name {char * first; char * last;}; // other method of creating struct but it doesnt use typedef.

// Opens a file, writes a line, then closes file.
int main(int argc, char * argv[])
{
    iii i = 5; // Since we use typedef, iii replaces int
    name n; // Instantiate name struct.

    name * sa[30]; // array of pointers to structs
    name sss[30]; // not a pointer

    sa[0] = malloc(sizeof(name)); // allocates name type size for one struct
    sa[0]->first = malloc(5); // dereference using pointer. Same as *(sa[0]).first

    sss[0].first; // Since sss isnt a pointer, we can use . notation since we dont need to dereference.





    n.first = malloc(25);
    strncpy(n.first, "firstName",25);    

    FILE * outFile; // 
    if((i = fopen_s(&outFile, "out.txt", "w"))) // Returns 0 if worked, else failed. Safe version of fopen.
    { // Output file variable, file name, permissions (read/write/etc)
        // After recovering from system call error, reset errno as it doesnt automatically reset.
        exit(-1); // fails
    } 
    // Unsafe version of fopen. Depreciated.
    //outFIe = fopen("out.txt", "w");
    //if(outf == NULL) exit(-1);
    
    fprintf(outf, "enter text here to output to file\n");

    fclose(outFile);
}

1           2       3       4       5           6               7               8           9           10          11      12              13                  14      15          16              17                    19    20
SongNumber,SongID,AlbumID,AlbumName,ArtistID,ArtistLatitude,ArtistLocation,ArtistLongitude,ArtistName,Dancibility,Duration,KeySignature,KeySignatureConfidence,Tempo,Hottttneessss,TimeSignature,TimeSignatureConfidence,Title,YEar

Need:
    4   AlbumName           char *
    7   ArtistLocation      char *
    9   ArtistName          char *
    11  Duration            says to use float (maybe use double)
    15  Hottttneessss       double
    19  Title               char *
    20  Year                int
    
