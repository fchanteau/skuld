import { Spinner } from 'reactstrap';

export type LoadingProps = {
  className?: string;
};

export function Loading(props: LoadingProps) {
  return (
    <div className="position-absolute top-0 h-100 w-100 d-flex flex-column justify-content-center align-items-center bg-light">
      <Spinner className={props.className} color="primary" type="grow">
        Loading...
      </Spinner>
    </div>
  );
}
