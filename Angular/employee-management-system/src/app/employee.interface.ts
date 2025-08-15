export interface EmployeeGoal {
  employeeId: number;
  title: string;
  description: string;
  targetDate: string;
  isCompleted: boolean;
  id: number;
}

export interface Employee {
  id: number;
  name: string;
  email: string;
  department: string;
  dateOfJoining: Date;
}

export interface PerformanceReview {
  employeeId: number;
  reviewDate: Date;
  rating: number;
  comments: string;
  reviewerName: string
}

