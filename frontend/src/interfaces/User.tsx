
export interface UserImageUploadResponse {
    imageUrl: string;
    others: string[];
}

export interface UserResponse {
    id: string;
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    rol: string;
}

export interface GetUserResponse {
    result: UserResponse;
}

export interface ListUserResponse {
    users: UserResponse[];
}


export interface UserImageUploadResponse {
    imageUrl: string;
    others: string[];
}

export interface EditUserData {
    id: string;
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    oldPassword: string;
    password: string;
    confirmPassword: string;
};

export interface Comment {
    username: string;
    rating: number;
    comment: string;
}

export interface CommentsContainerProps {
    comments: Comment[];
    invisible: boolean;
}