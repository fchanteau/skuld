import { type SerializedError } from "@reduxjs/toolkit";
import { type FetchBaseQueryError } from "@reduxjs/toolkit/dist/query/react";
import { useMemo } from "react";
import { Alert } from "reactstrap";

interface ErrorProps {
    error: FetchBaseQueryError | SerializedError;
}

interface ErrorModel {
    detail: string;
    extensions: any;
    instance: string;
    status: number;
    title: string;
    type: string;
}

export function ErrorMessage(props: ErrorProps) {
    const { error } = props;

    const message = useMemo(() => {
        if ('status' in error) {
            // you can access all properties of `FetchBaseQueryError` here
            if (error.status === 'FETCH_ERROR') {
                console.error(error);
                return "Oops ! Something bad happened. Contact admin if the problem persist."
            }

            return (error.data as ErrorModel).detail;  
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