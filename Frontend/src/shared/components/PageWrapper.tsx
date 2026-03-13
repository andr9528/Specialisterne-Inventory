import type { PropsWithChildren } from "react"

const PageWrapper = ({ children }: PropsWithChildren) => {
    return (
        <div className="pt-20 min-h-screen mx-(--default-margin-x)">
            {children}
        </div>
    )
}

export default PageWrapper;