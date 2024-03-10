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

      init() {
        self.id = UUID(uuidString: "00000000-0000-0000-0000-000000000000")!
        self.title = nil
        self.startDate = nil
        self.endDate = nil
        self.targetEndDate = nil
        self.lastModified = nil
        self.subTasks = nil
        self.status = nil
    }

      init(from decoder: Decoder) throws {
        
        let container = try decoder.container(keyedBy: CodingKeys.self)
        id = try container.decode(UUID.self, forKey: .id)
        title = try container.decodeIfPresent(String.self, forKey: .title)
          let dateFormatter = DateFormatter()

        
          do {
              startDate = try container.decodeIfPresent(Date.self, forKey: .startDate)
          } catch {
              //print("Error decoding startDate: \(error)")
              startDate = nil
          }

          do {
              endDate = try container.decodeIfPresent(Date.self, forKey: .endDate)
          } catch {
              print("Error decoding endDate: \(error), \(try container.decodeIfPresent(String.self, forKey: .endDate) ?? "")")
              endDate = nil
          }

          do {
              targetEndDate = try container.decodeIfPresent(Date.self, forKey: .targetEndDate)
          } catch {
              print("Error decoding targetEndDate: \(error), \(try container.decodeIfPresent(String.self, forKey: .targetEndDate) ?? "Unknown key")")
              targetEndDate = nil
          }
          do {
              let lastModifiedString = try container.decodeIfPresent(String.self, forKey: .lastModified)

              dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSSZ"
                      if let dateString = lastModifiedString {
                          lastModified = dateFormatter.date(from: dateString)
                      } else {
                          lastModified = nil
                      }
          } catch {
              print("Error decoding lastModified: \(error)")
              lastModified = nil
          }
        
        subTasks = try container.decodeIfPresent([SubTask].self, forKey: .subTasks)
        status = try container.decodeIfPresent(Int.self, forKey: .status)
    }
}
