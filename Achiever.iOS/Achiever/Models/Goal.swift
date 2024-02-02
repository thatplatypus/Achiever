//
//  Goal.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
struct Goal: Codable, Identifiable {
    let id: UUID
    let title: String?
    let startDate, endDate, targetEndDate, lastModified: Date?
    let subTasks: [SubTask]?
    let status: Int?
}
