import { SerializedError } from "@reduxjs/toolkit";
import { FetchBaseQueryError } from "@reduxjs/toolkit/dist/query/react";
import { useMemo } from "react";
import { Alert } from "reactstrap";

interface ErrorProps {
    error: FetchBaseQueryError | SerializedError;
}

interface ErrorModel {
    Detail: string;
    Extensions: any;
    Instance: string;
    Status: number;
    Title: string;
    Type: string;
}

export function Error(props: ErrorProps) {
    const { error } = props;

    const message = useMemo(() => {
        if ('status' in error) {
            // you can access all properties of `FetchBaseQueryError` here
            if (error.status === 'FETCH_ERROR') {
                console.error(error);
                return "Oops ! Something bad happened. Contact admin if the problem persist."
            }

           return (error.data as ErrorModel).Detail;
    
          }
          else {
              // you can access all properties of `SerializedError` here
              return error.message
          }
    }, [error]);

    return <Alert color="danger">
        {message}
    </Alert>
}