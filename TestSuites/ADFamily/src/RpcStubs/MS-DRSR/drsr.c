#include "ms-drsr.h"

/* Allocate the Memory pointed by */
void *__RPC_USER MIDL_user_allocate(size_t memSize)
{
    return malloc(memSize);
}

/* De-allocate the Memory pointed by */
void __RPC_USER MIDL_user_free(void *p)
{
     free(p);
}

void __RPC_USER DRS_HANDLE_rundown( DRS_HANDLE p )
{
}