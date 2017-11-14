// Add dtc.c in the assitional sources in MAKEFILE

note_t *HEAD = null
note_t *curr = null;

int main(void)
{
    HEAD = malloc(sizeof(note_t));
    HEAD->pitch = 0;
    HEAD->next = NULL;

    cur = HEAD;


    initialize_dtc(INCH_4, &TA0CCR0);   
    // this gives type mismatch
    // in header file change second var to volatile.
    // INCH_4 is analog channel for input
    // result is (0-1023)
    // can set predefined note range and associate the result to the note.
    // 1,000,000/8/1023 = 122 (maybe 122Hz or 244Hz depends if note is full cycle or half cycle of square wave)

    for(;;)
    {

    }

    button interrupt
    {   
        cur->next = malloc(sizeof(note_t));
        cur->pitch = TA0CCR0;
        cur = cur->next;
        // maybe not needed
        cur->next = NULL;
        cur->pitch = 0;
    }

}