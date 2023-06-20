import { SkuldLogo } from "@/components/shared";
import { isConnected } from "@/store/auth";
import { useEffect } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";

export function Home() {
    const navigate = useNavigate();

    const userIsConnected = useSelector(isConnected);

    useEffect(() => {
        if (!userIsConnected) {
            console.log("FCU go to landing");
            navigate('/landing');
        }
    }, [userIsConnected]);

    return (
        <div className='home d-flex skuld-height justify-content-center align-items-center text-center'>
            <SkuldLogo width={500} height={500} />
        </div>
    )
}