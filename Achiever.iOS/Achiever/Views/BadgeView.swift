//
//  BadgeView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI

struct BadgeView: View {
    var text: String
    var color: Color

    var body: some View {
        Text(text)
            .font(.caption)
            .fontWeight(.bold)
            .padding(.horizontal, 8)
            .padding(.vertical, 4)
            .background(color)
            .foregroundColor(.white)
            .clipShape(Capsule())
            .lineLimit(1)
            .truncationMode(.tail)
    }
}
