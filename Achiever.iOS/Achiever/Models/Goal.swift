//
//  Goal.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
struct Goal: Codable, Identifiable {
    let id: UUID
    var title: String?
    var startDate, endDate, targetEndDate, lastModified: Date?
    var subTasks: [SubTask]?
    var status: Int?
}
