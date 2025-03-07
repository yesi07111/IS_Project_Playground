export interface OnlinePagesProps {
    online: boolean;
    useCaptcha: boolean;
}

export interface DataPagesProps {
    reload: boolean;
}

export interface StatsSummaryProps {
    visitants: number;
    activeActivities: number;
    rating: number;
}

export interface GetHomePageInfoResponse {
    visitors: number
    activeActivities: number
    score: number
}