//
//  SubtaskCardView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/4/24.
//

import Foundation
import SwiftUI

import SwiftUI

struct SubtaskCardView: View {
    var title: String
    var hours: Double
    var status: String
    var hasNotes: Bool
    var color: Color
    
    var body: some View {
            VStack(alignment: .leading) {
                Text(title)
                    .font(.footnote)
                    .lineLimit(1)
                    .truncationMode(.tail)
                    .foregroundColor(.gray)

                BadgeView(text: convertStatus(status: status), color: color)
                Spacer()
                HStack {
                    Image(systemName: "note.text")
                        .foregroundColor(hasNotes ? .primary : .gray)
                    Image(systemName: "clock")
                        .foregroundColor(hours > 0 ? .primary : .gray)
                    Text("\(String(format: "%.2g", hours))")
                        .font(.subheadline)
                        .font(.system(size: 10))
                        .foregroundColor(.gray)
                }
            }
            .padding(4)
            .frame(width: 95, height: 100)
            .background(Color(UIColor.systemBackground))
            .cornerRadius(5)
        }
    
    func convertStatus(status: String) -> String {
        switch status.lowercased() {
        case "new":
            return "New"
        case "inprogress":
            return "In Progress"
        case "completed":
            return "Completed"
        default:
            return status
        }
    }
}
