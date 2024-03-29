//
//  SubTask.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
struct SubTask: Codable, Identifiable {
    let id: UUID
    var title: String?
    var status: String?
    var lastModified: Date?
    let goalId: UUID
    var estimatedHours: Double?
    var note: String?
    var order: Int?
    var userDeleted: Bool?
    
    enum CodingKeys: String, CodingKey {
           case id, title, status, lastModified, goalId, estimatedHours, note, order, userDeleted
       }
    
    init(from decoder: Decoder) throws {
         let container = try decoder.container(keyedBy: CodingKeys.self)
         id = try container.decode(UUID.self, forKey: .id)
         title = try container.decode(String.self, forKey: .title)
         status = try container.decode(String.self, forKey: .status)
         

        let lastModifiedString = try container.decode(String.self, forKey: .lastModified)
        let dateFormatter = DateFormatter()

        if lastModifiedString == "0001-01-01T00:00:00" {
            lastModified = Date.distantPast
        } else {
            dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSSZ"
            if let lastModifiedDate = dateFormatter.date(from: lastModifiedString) {
                lastModified = lastModifiedDate
            } else {
                dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ssZ"
                if let lastModifiedDate = dateFormatter.date(from: lastModifiedString) {
                    lastModified = lastModifiedDate
                } else {
                    lastModified = nil
                }
            }
        }
        
        goalId = try container.decode(UUID.self, forKey: .goalId)
        estimatedHours = try container.decodeIfPresent(Double.self, forKey: .estimatedHours)
        note = try container.decodeIfPresent(String.self, forKey: .note)
        order = try container.decodeIfPresent(Int.self, forKey: .order)
        userDeleted = try container.decodeIfPresent(Bool.self, forKey: .userDeleted)


     }

        init() {
        self.id = UUID(uuidString: "00000000-0000-0000-0000-000000000000")!
        self.title = nil
        self.status = "New"
        self.lastModified = nil
        self.goalId = UUID()
        self.estimatedHours = nil
        self.note = nil
        self.order = nil
        self.userDeleted = nil
    }

}
