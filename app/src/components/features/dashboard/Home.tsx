import { SkuldLogo } from "@/components/shared";
import S from "./Home.module.scss";

export function Home() {
    return (
        <div className={`${S.home} d-flex skuld-height justify-content-center align-items-center text-center`}>
            <SkuldLogo width={500} height={500} />
        </div>
    )
}