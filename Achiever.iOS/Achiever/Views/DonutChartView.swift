//
//  DonutChartView.swift
//  Achiever
//
//  Created by Tom Brewer on 2/1/24.
//

import Foundation
import SwiftUI
struct DonutChartView: View {
    var ratio: Double
    var height: CGFloat
    var width: CGFloat

    var body: some View {
        ZStack {
            Circle()
                .stroke(lineWidth: 10)
                .opacity(0.3)
                .foregroundColor(.gray)

            Path { path in
                let radius = min(width, height) / 2 // use the same radius as the circle
                let center = CGPoint(x: width / 2, y: height / 2)
                path.addArc(center: center, radius: radius, startAngle: .degrees(-90), endAngle: .degrees(360 * ratio - 90), clockwise: false)
            }
            .stroke(style: StrokeStyle(lineWidth: 10, lineCap: .round, lineJoin: .round))
            .foregroundColor(ratio == 1 ? .green : Color(red: 0.0, green: 0.4 + 0.3 * ratio, blue: 0.3 + 0.7 * ratio)) // adjust blue intensity based on ratio

            Text("\(Int(ratio * 100))%")
                .font(.system(size: 20)) // adjust the font size here
                .foregroundColor(.gray) // adjust the text color here
                .bold()
        }
        .frame(width: width, height: height)
    }
}

