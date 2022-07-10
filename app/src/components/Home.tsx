import { Button } from "reactstrap";

export function Home() {
    return (
        <div className="home skuld-height d-flex justify-content-center align-items-center text-center">
            <div>
                <h1 className="display-1">Skuld</h1>
                <p>The best companion for small and medium businesses</p>
                <div className="action-buttons">
                    <Button color="primary mx-1">
                        Sign In
                    </Button>
                    <Button color="primary mx-1" outline>
                        Sign Up
                    </Button>
                </div>
            </div>
        </div>
    )
}