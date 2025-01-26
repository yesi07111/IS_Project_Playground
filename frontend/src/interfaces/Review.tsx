export interface Review {
    reviewId: string;
    activityId: string;
    activityName: string;
    comment: string;
    rating: number;
}

export interface ListReviewResponse {
    result: Review[];
}

export interface ReviewData {
    UserId: string;
    ActivityDateId: string;
    Comment: string;
    Rating: number;
}
