export interface User {
    id: string;
    name: string;
    lastName: string;
    userName: string;
    role: string;
}

export interface ListUserResponse{
    result: User[];
}